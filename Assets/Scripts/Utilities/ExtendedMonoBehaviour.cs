using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities
{
    public class ExtendedMonoBehaviour : MonoBehaviour
    {
        protected CoroutineBuilder Coroutine()
        {
            return gameObject.AddComponent<CoroutineBuilder>();
        }

        protected class CoroutineBuilder : MonoBehaviour
        {
            private readonly Queue<CoroutineStep> _steps = new Queue<CoroutineStep>();

            private bool _destroyOnFinish = true;
            public bool IsRunning { get; private set; }

            private void OnDisable()
            {
                Cancel();
            }

            public CoroutineBuilder Invoke(UnityAction action)
            {
                _steps.Enqueue(new CoroutineStep(CoroutineType.Invoke, action));
                return this;
            }

            public CoroutineBuilder WaitForSeconds(float seconds)
            {
                _steps.Enqueue(new CoroutineStep(CoroutineType.WaitForSeconds, seconds));
                return this;
            }

            public CoroutineBuilder ForTimes(int times)
            {
                _steps.Enqueue(new CoroutineStep(CoroutineType.ForTimes, times));
                return this;
            }

            public CoroutineBuilder While(Func<bool> predicate)
            {
                _steps.Enqueue(new CoroutineStep(CoroutineType.While, predicate));
                return this;
            }

            public void Run()
            {
                StartCoroutine(RunCoroutines());
            }

            public CoroutineBuilder WaitForEndOfFrame()
            {
                _steps.Enqueue(new CoroutineStep(CoroutineType.WaitForEndOfFrame, null));
                return this;
            }

            public CoroutineBuilder WaitForFixedUpdate()
            {
                _steps.Enqueue(new CoroutineStep(CoroutineType.WaitForFixedUpdate, null));
                return this;
            }

            public CoroutineBuilder WaitUntil(Func<bool> predicate)
            {
                _steps.Enqueue(new CoroutineStep(CoroutineType.WaitUntil, predicate));
                return this;
            }

            public CoroutineBuilder WaitWhile(Func<bool> predicate)
            {
                _steps.Enqueue(new CoroutineStep(CoroutineType.WaitWhile, predicate));
                return this;
            }

            public CoroutineBuilder DestroyOnFinish(bool condition = true)
            {
                _destroyOnFinish = condition;
                return this;
            }

            public void Cancel()
            {
                StopCoroutine(RunCoroutines());
                IsRunning = false;
                if (_destroyOnFinish) Destroy(this);
            }

            private IEnumerator RunCoroutines()
            {
                IsRunning = true;

                var iterations = 0;
                var start = 0;

                for (var i = 0; i < _steps.Count; i++)
                {
                    var step = _steps.ElementAt(i);

                    switch (step.Type)
                    {
                        case CoroutineType.Invoke:
                            ((UnityAction) step.Value).Invoke();
                            break;
                        case CoroutineType.WaitForSeconds:
                            yield return StartCoroutine(RunWaitForSeconds((float) step.Value));
                            break;
                        case CoroutineType.WaitForEndOfFrame:
                            yield return StartCoroutine(RunWaitForEndOfFrame());
                            break;
                        case CoroutineType.WaitForFixedUpdate:
                            yield return StartCoroutine(RunWaitForFixedUpdate());
                            break;
                        case CoroutineType.WaitUntil:
                            yield return StartCoroutine(RunWaitUntil((Func<bool>) step.Value));
                            break;
                        case CoroutineType.WaitWhile:
                            yield return StartCoroutine(RunWaitWhile((Func<bool>) step.Value));
                            break;
                        case CoroutineType.ForTimes when iterations < (int) step.Value - 1:
                            i = start - 1;
                            iterations++;
                            break;
                        case CoroutineType.ForTimes:
                            iterations = 0;
                            start = i + 1;
                            break;
                        case CoroutineType.While when ((Func<bool>) step.Value).Invoke():
                            i = start - 1;
                            iterations++;
                            break;
                        case CoroutineType.While:
                            iterations = 0;
                            start = i + 1;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                IsRunning = false;
                if (_destroyOnFinish) Destroy(this);
            }

            private static IEnumerator RunWaitForSeconds(float seconds)
            {
                yield return new WaitForSeconds(seconds);
            }

            private static IEnumerator RunWaitForEndOfFrame()
            {
                yield return new WaitForEndOfFrame();
            }

            private static IEnumerator RunWaitForFixedUpdate()
            {
                yield return new WaitForFixedUpdate();
            }

            private static IEnumerator RunWaitUntil(Func<bool> predicate)
            {
                yield return new WaitUntil(predicate);
            }

            private static IEnumerator RunWaitWhile(Func<bool> predicate)
            {
                yield return new WaitWhile(predicate);
            }

            private enum CoroutineType
            {
                Invoke,
                WaitForSeconds,
                WaitForEndOfFrame,
                ForTimes,
                While,
                WaitForFixedUpdate,
                WaitUntil,
                WaitWhile
            }

            private readonly struct CoroutineStep
            {
                public readonly CoroutineType Type;
                public readonly object Value;

                public CoroutineStep(CoroutineType type, object value)
                {
                    Type = type;
                    Value = value;
                }
            }
        }
    }
}